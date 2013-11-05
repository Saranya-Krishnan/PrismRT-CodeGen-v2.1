The focus of PrismRTCodeGen_v2.1 is data validation both client-side and server-side. This update takes advantage of 'Prism for Windows Runtime' framework for:
	1) Validating data via ValidationAttributes on Model properties
	2) Saving validation errors when app suspends
	3) Restoring validation errors when app is restarted

Changes to PrismRTCodeGen templates:
	1) Added FormsValidation sample to download.
		a. UILogic->Models->Client-side validation
		b. WebAPI->Models->Server-side validation
		c. Specify validation rules with ValidationAttributes on  'Model' properties 
		d. Bind UI with Errors collection messages
		e. Attach UI behavior to highlight fields in error
		f. Perform client-side validation when tabbing to next field and validate all fields on Submit
		g. Use a number of RegEx validation patterns
		h. Save UI errors state on Suspension and restore when app restarted
	2) DalInterface, DalEF4, DalMemory projects removed. Code moved to WebAPI.
	3) EFxDataModel added. Contains only .edmx.
	4) WebAPI->Models
		a. CustomValidation server-side
		b. Example of 'partial' classes for ValidateZipCode and ValidateState
		c. Example of dependent properties
	5) UILogic->ViewModels->DetailPageViewModel
		a. Override OnNavigatedFrom to save "errorsCollection"
		b. Override OnNavigatedTo to restore "errorsCollection"
	6) UILayer->Views->ListPage
		a. GridView changed to ListView
		b. Override SaveState to save scrollViewer.VerticalOffset
		c. Override LoadState to restore scrollViewer.VerticalOffset
	7) UILayer->Common
		a. Added DependencyPropertyChangedHelper.cs. Used to support save/restore scrollViewer.VerticalOffset.