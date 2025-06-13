#include "StdAfx.h"
#include "ctiform.h"
#include "CTIEditBox.h"
#include "CTICheckbox.h"
#include "CTIButton.h"
#include "CTIStatic.h"
#include "CTIInfoField.h"

CCTIForm::CCTIForm(void)
{
}

CCTIForm::~CCTIForm(void)
{
	for (IdLabelObjectList::iterator it = m_listChildren.begin();it!=m_listChildren.end();it++) {
		CCTIIdLabelObject* pObject = *it;
		delete pObject;
	}
	m_listChildren.clear();
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIForm::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlForm= pXMLDoc->createElement(GetTag());

		AddAttributesToElement(pXMLDoc,pXmlForm);

		//Output all the child objects of children of this XML element in the order in which they were added.
		for (IdLabelObjectList::iterator it = m_listChildren.begin();it!=m_listChildren.end();it++) {
			CCTIIdLabelObject* pObject = *it;
			AddChildIfVisible(pXMLDoc, pXmlForm, pObject);
		}

		pFragment->appendChild(pXmlForm);
		pXmlForm.Release();
	} else ATLTRACE(L"Form not visible!");
	return pFragment;
}

void CCTIForm::AddChild(CCTIIdLabelObject* pChild)
{
	//The children of a form should always be visible -- the form itself controls the visibility.
	pChild->SetVisible(true);

	m_listChildren.push_back(pChild);
}

CCTIStatic* CCTIForm::AddStatic(std::wstring sId, std::wstring sLabel) 
{
	CCTIStatic* pStatic = new CCTIStatic(sId,sLabel);
	AddChild(pStatic);

	return pStatic;
}

CCTIInfoField* CCTIForm::AddInfoField(std::wstring sId, std::wstring sLabel, std::wstring sValue) 
{
	CCTIInfoField* pInfoField = new CCTIInfoField(sId,sLabel);
	pInfoField->SetValue(sValue);
	AddChild(pInfoField);

	return pInfoField;
}

CCTIEditBox* CCTIForm::AddEditBox(std::wstring sId, std::wstring sLabel, std::wstring sValue) 
{
	CCTIEditBox* pEditBox = new CCTIEditBox(sId,sLabel);
	pEditBox->SetValue(sValue);
	AddChild(pEditBox);

	return pEditBox;
}

CCTICheckbox* CCTIForm::AddCheckbox(std::wstring sId, std::wstring sLabel, bool bChecked) 
{
	CCTICheckbox* pCheckbox = new CCTICheckbox(sId,sLabel);
	pCheckbox->SetChecked(bChecked);
	AddChild(pCheckbox);

	return pCheckbox;
}

CCTIButton* CCTIForm::AddButton(std::wstring sId, std::wstring sLabel) 
{
	CCTIButton* pButton = new CCTIButton(sId,sLabel);
	AddChild(pButton);

	return pButton;
}

CCTIIdLabelObject* CCTIForm::GetChildById(std::wstring& sId)
{
	for (IdLabelObjectList::iterator listIt = m_listChildren.begin(); listIt!=m_listChildren.end(); listIt++) {
		if ((*listIt)->GetId()==sId) {
			return(*listIt);
		}
	}
	return NULL;
}

void CCTIForm::PopulateFormData(PARAM_MAP& mapFormData)
{
	for (PARAM_MAP::iterator it=mapFormData.begin();it!=mapFormData.end();it++) {
		std::wstring sId = it->first;
		CCTIIdLabelObject* pObject = GetChildById(sId);

		if (pObject) {
			//Different visual items have different means of setting values...

			//The dynamic cast will just return null if the object is not actually an edit box.
			CCTIEditBox* pEditBox = dynamic_cast<CCTIEditBox*>(pObject);
			if (pEditBox) {
				pEditBox->SetValue(it->second);
				//We're done -- move on with the looping
				continue;
			}

			CCTICheckbox* pCheckbox = dynamic_cast<CCTICheckbox*>(pObject);
			if (pCheckbox) {
				pCheckbox->SetChecked(it->second==KEY_TRUE);
				//We're done -- move on with the looping
				continue;
			}
		}
	}
}
